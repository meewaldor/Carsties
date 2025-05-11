'use client';
import Heading from '@/components/Heading';
import React from 'react';
import CountdownTimer from '@/features/auctions/components/CountdownTimer';
import CardImage from '@/features/auctions/components/CardImage';
import { useGetBidsForAuctionQuery } from '@/features/bids/api/BidApi';
import { useGetAuctionDetailsQuery } from '@/features/auctions/api/AuctionApi';
import DetailedSpecs from './DetailedSpecs';
import EditButton from './EditButton';
import DeleteButton from './DeleteButton';

type Props = {
  id: string;
  user: any;
};

export default function Details({ id, user }: Props) {
  const { data: auction, isLoading } = useGetAuctionDetailsQuery(id);
  const { data: bids } = useGetBidsForAuctionQuery(id);

  if (!auction || isLoading) return <div>Loading...</div>;

  return (
    <div>
      <div className='flex justify-between'>
        <div className='flex items-center gap-3'>
          <Heading title={`${auction.make} ${auction.model}`} />
          {user?.username === auction.seller && (
            <>
              <EditButton id={auction.id} />
              <DeleteButton id={auction.id} />
            </>
          )}
        </div>

        <div className='flex gap-3'>
          <h3 className='text-2xl font-semibold'>Time remaining:</h3>
          <CountdownTimer auctionEnd={auction.auctionEnd} />
        </div>
      </div>

      <div className='grid grid-cols-2 gap-6 mt-3'>
        <div className='w-full bg-gray-200 relative aspect-[4/3] rounded-lg overflow-hidden'>
          <CardImage imageUrl={auction.imageUrl} />
        </div>
        <div className='border-2 rounded-lg p-2 bg-gray-100'>
          <Heading title='Bids' />
          {bids?.map((bid) => (
            <p key={bid.id}>
              {bid.bidder} - {bid.amount}
            </p>
          ))}
        </div>
      </div>

      <div className='mt-3 grid grid-cols-1 rounded-lg'>
        <DetailedSpecs auction={auction} />
      </div>
    </div>
  );
}
