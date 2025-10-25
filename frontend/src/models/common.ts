export interface PaginatedResponse<T> {
  items: Array<T>;
  page: number;
  pageSize: number;
  totalItemsCount: number;
}

export interface PaginationQuery {
  page: number;
  pageSize: number;
}
