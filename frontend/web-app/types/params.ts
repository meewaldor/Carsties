export type AuctionParams = {
  pageNumber: number;
  pageSize: number;
  searchTerm: string;
  orderBy: string;
  filterBy: string;
  seller?: string;
  winner?: string;
};
