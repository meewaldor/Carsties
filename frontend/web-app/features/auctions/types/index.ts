export type Auction = {
  reservePrice: number;
  seller: string;
  winner?: string;
  soldAmount: number;
  currentHighBid: number;
  createdAt: string;
  updatedAt: string;
  auctionEnd: string;
  status: string;
  make: string;
  model: string;
  year: number;
  color: string;
  mileage: number;
  imageUrl: string;
  id: string;
};

export interface CreateAuction {
  make: string
  model: string
  color: string
  year: number
  mileage: number
  imageUrl: string
  reservePrice: number
  auctionEnd: string
}
