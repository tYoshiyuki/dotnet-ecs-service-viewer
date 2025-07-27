import { CommonModule } from '@angular/common';
import { Component, effect, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ConfirmationDialogComponent } from '@app/components/confirmation-dialog/confirmation-dialog.component';
import { EcsClusterNames, EcsServiceInfo, UpdateEcsServiceDesiredCountRequest } from '@app/models/ecs-model';
import { EcsService } from '@app/services/ecs.service';
import { of } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-ecs-service-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatSelectModule,
    MatFormFieldModule
  ],
  templateUrl: './ecs-service-list.component.html',
  styleUrls: ['./ecs-service-list.component.scss']
})
export class EcsServiceListComponent implements OnInit {
  displayedColumns: string[] = ['serviceName', 'clusterName', 'status', 'runningCount', 'desiredCount', 'actions'];
  dataSource = new MatTableDataSource<EcsServiceInfo>();
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  clusterNames = signal<EcsClusterNames>([]);
  selectedCluster = signal<string | null>(null);

  constructor(
    private ecsService: EcsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    // selectedCluster の変更を監視し、サービスを再ロードするエフェクト
    effect(() => {
      const cluster = this.selectedCluster();
      if (cluster) {
        this.loadEcsServices(cluster);
      }
    });
  }

  ngOnInit(): void {
    this.loadEcsClusterNames();
  }

  /**
   * ECSクラスター名一覧を取得します。
   */
  loadEcsClusterNames(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.ecsService.getEcsClusterNames().pipe(
      catchError(err => {
        console.error('ECSクラスター名の取得中にエラーが発生しました:', err);
        this.errorMessage.set('ECSクラスター名の読み込みに失敗しました。');
        this.snackBar.open('ECSクラスター名の読み込みに失敗しました。', '閉じる', { duration: 3000 });
        return of([]);
      }),
      finalize(() => this.isLoading.set(false))
    ).subscribe(names => {
      this.clusterNames.set(names);
      // 初回ロード時、クラスターが一つでもあれば最初のものを選択
      if (names.length > 0 && !this.selectedCluster()) {
        this.selectedCluster.set(names[0]);
      }
    });
  }

  /**
   * 指定されたクラスターのECSサービス一覧を取得します。
   * @param clusterName 対象のクラスター名
   */
  loadEcsServices(clusterName: string): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.dataSource.data = [];
    this.ecsService.getEcsServices(clusterName).pipe(
      catchError(err => {
        console.error(`クラスター '${clusterName}' のECSサービスの取得中にエラーが発生しました:`, err);
        this.errorMessage.set(`クラスター '${clusterName}' のECSサービスの読み込みに失敗しました。`);
        this.snackBar.open(`サービス読み込みに失敗しました。`, '閉じる', { duration: 3000 });
        return of([]);
      }),
      finalize(() => this.isLoading.set(false))
    ).subscribe(services => {
      this.dataSource.data = services;
    });
  }

  refreshServices(): void {
    const currentCluster = this.selectedCluster();
    if (currentCluster) {
      this.loadEcsServices(currentCluster);
      this.snackBar.open('サービス情報を更新しました。', '閉じる', { duration: 2000 });
    }
  }

  /**
   * サービス起動確認ダイアログを表示し、ユーザーの選択に応じてサービスを起動します。
   * @param service 対象のECSサービス
   */
  confirmStartService(service: EcsServiceInfo): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '350px',
      data: { title: 'サービスの起動確認', message: `[${service.serviceName}] サービスを起動しますか？` }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateServiceDesiredCount(service, 1, '起動');
      }
    });
  }

  /**
   * サービス停止確認ダイアログを表示し、ユーザーの選択に応じてサービスを停止します。
   * @param service 対象のECSサービス
   */
  confirmStopService(service: EcsServiceInfo): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '350px',
      data: { title: 'サービスの停止確認', message: `[${service.serviceName}] サービスを停止しますか？` }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateServiceDesiredCount(service, 0, '停止');
      }
    });
  }

  /**
   * ECSサービスのDesiredCountを更新します。
   * @param service 対象のECSサービス
   * @param desiredCount 新しいDesiredCount
   * @param operationName 操作名（例: '起動', '停止'）
   */
  updateServiceDesiredCount(service: EcsServiceInfo, desiredCount: number, operationName: string): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const request: UpdateEcsServiceDesiredCountRequest = {
      clusterName: this.extractClusterNameFromArn(service.clusterArn), // ARNからクラスター名を抽出
      serviceName: service.serviceName,
      desiredCount: desiredCount
    };

    this.ecsService.updateEcsServiceDesiredCount(request).pipe(
      catchError(err => {
        console.error(`サービス [${service.serviceName}] の${operationName}中にエラーが発生しました:`, err);
        this.errorMessage.set(`[${service.serviceName}] の${operationName}に失敗しました。`);
        this.snackBar.open(`[${service.serviceName}] の${operationName}に失敗しました。`, '閉じる', { duration: 3000 });
        return of(null);
      }),
      finalize(() => this.isLoading.set(false))
    ).subscribe(updatedService => {
      if (updatedService) {
        this.snackBar.open(`[${service.serviceName}] を${operationName}しました。`, '閉じる', { duration: 3000 });
        // UIを更新するため、現在のクラスターのサービスを再ロード
        if (this.selectedCluster()) {
          this.loadEcsServices(this.selectedCluster()!);
        }
      }
    });
  }

  /**
   * Cluster ARN から Cluster Name を抽出するヘルパー関数
   * 例: arn:aws:ecs:{region}:{account-id}:cluster/{cluster-name} -> {cluster-name}
   */
  public extractClusterNameFromArn(clusterArn: string): string {
    const parts = clusterArn.split('/');
    return parts.length > 1 ? parts[parts.length - 1] : clusterArn;
  }
}