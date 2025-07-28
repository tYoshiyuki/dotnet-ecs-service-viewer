/**
 * バックエンドAPIへのDesiredCount更新リクエストボディに対応するインターフェース
 */
export interface UpdateEcsServiceDesiredCountRequest {
  clusterName: string;
  serviceName: string;
  desiredCount: number;
}