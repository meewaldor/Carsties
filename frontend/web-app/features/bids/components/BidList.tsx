import { Auction } from '@/types';
import { User } from 'next-auth';
import React from 'react';
import { useGetBidsForAuctionQuery } from '../api/BidApi';
import Heading from '@/components/Heading';
import BidItem from './BidItem';
import { numberWithCommas } from '@/lib/utils';
import EmptyFilter from '@/components/EmptyFilter';
import BidForm from './BidForm';

type Props = {
  user: User | null;
  auction: Auction;
};
export default function BidList({ auction, user }: Props) {
  const { data: bids, isLoading } = useGetBidsForAuctionQuery(auction.id);
  const highBid = bids?.reduce(
    (prev, current) =>
      prev > current.amount
        ? prev
        : current.bidStatus.includes('Accepted')
        ? current.amount
        : prev,
    0
  );
  if (!bids || isLoading) return <span>Loading bids...</span>;

  return (
    <div className='rounded-lg shadow-md'>
      <div className='py-2 px-4 bg-white'>
        <div className='sticky top-0 bg-white p-2'>
          <Heading
            title={`Current high bid is $${numberWithCommas(
              highBid as number
            )}`}
          />
        </div>
      </div>

      <div className='overflow-auto h-[400px] flex flex-col-reverse px-2'>
        {bids?.length === 0 ? (
          <EmptyFilter
            title='No bids for this item'
            subtitle='Please feel free to make a bid'
          />
        ) : (
          <>
            {bids.map((bid) => (
              <BidItem key={bid.id} bid={bid} />
            ))}
          </>
        )}
      </div>

      <div className='px-2 pb-2 text-gray-500'>
        {!open ? (
          <div className='flex items-center justify-center p-2 text-lg font-semibold'>
            This auction has finished
          </div>
        ) : !user ? (
          <div className='flex items-center justify-center p-2 text-lg font-semibold'>
            Please login to make a bid
          </div>
        ) : user && user.username === auction.seller ? (
          <div className='flex items-center justify-center p-2 text-lg font-semibold'>
            You cannot bid on your own auction
          </div>
        ) : (
          <BidForm auctionId={auction.id} highBid={highBid as number} />
        )}
      </div>
    </div>
  );
}
