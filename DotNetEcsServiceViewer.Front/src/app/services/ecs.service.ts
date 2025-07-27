import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EcsClusterNames, EcsServiceInfo, UpdateEcsServiceDesiredCountRequest } from '@app/models/ecs-model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EcsService {

  private apiUrl = '/api/ecs'; // APIコントローラのルートパスに合わせる

  constructor(private http: HttpClient) { }

  /**
   * 全てのECSクラスター名を取得します。
   */
  getEcsClusterNames(): Observable<EcsClusterNames> {
    return this.http.get<EcsClusterNames>(`${this.apiUrl}/get-ecs-cluster-names`);
  }

  /**
   * 指定されたクラスターのECSサービス一覧を取得します。
   * @param clusterName 対象のクラスター名
   */
  getEcsServices(clusterName: string): Observable<EcsServiceInfo[]> {
    return this.http.get<EcsServiceInfo[]>(`${this.apiUrl}/get-ecs-services`, {
      params: { clusterName: clusterName }
    });
  }

  /**
   * ECSサービスのDesiredCountを更新します（起動/停止）。
   * @param request 更新リクエストオブジェクト
   */
  updateEcsServiceDesiredCount(request: UpdateEcsServiceDesiredCountRequest): Observable<EcsServiceInfo> {
    // POSTリクエストでリクエストボディを送信
    return this.http.post<EcsServiceInfo>(`${this.apiUrl}/update-ecs-service-desired-count`, request);
  }
}
