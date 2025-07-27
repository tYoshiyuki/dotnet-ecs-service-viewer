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

/**
 * バックエンドAPIから返されるクラスタ名リストに対応するインターフェース（シンプルにstringの配列）
 */
export type EcsClusterNames = string[];

/**
 * バックエンドAPIへのDesiredCount更新リクエストボディに対応するインターフェース
 */
export interface UpdateEcsServiceDesiredCountRequest {
  clusterName: string;
  serviceName: string;
  desiredCount: number;
}