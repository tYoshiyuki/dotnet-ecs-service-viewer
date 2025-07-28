/**
 * バックエンドAPIから返されるECS Service DTOに対応するインターフェース
 * Amazon.ECS.Model.Service の主要なプロパティに基づいています。
 */
export interface EcsServiceInfo {
  serviceArn: string;
  serviceName: string;
  clusterArn: string;
  status: string;
  runningCount: number;
  desiredCount: number;
  pendingCount: number;
  createdAt: string; // Date型で受け取りますが、APIからはISO文字列として来る可能性が高いのでstringで受け取って変換します
  // 必要に応じて他のプロパティを追加
}