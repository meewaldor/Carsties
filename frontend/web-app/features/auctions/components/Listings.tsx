'use client';
import React, { useEffect, useState } from 'react';
import AuctionCard from './AuctionCard';
import Filters from './Filters';
import { useGetAuctionsQuery } from '../api/AuctionApi';
import EmptyFilter from '@/components/EmptyFilter';
import AppPagination from '@/components/AppPagination';
import { useAppDispatch, useAppSelector } from '@/redux/hooks';
import { setPageNumber } from '../slices/auctionSlice';

export default function Listings() {
  const auctionParams = useAppSelector((state) => state.auction);
  const { data: auctions, isLoading } = useGetAuctionsQuery(auctionParams);
  const dispatch = useAppDispatch();

  if (isLoading) return <h3>Loading...</h3>;

  return (
    <>
      <Filters />
      {!auctions ? (
        <EmptyFilter showReset />
      ) : (
        <>
          <div className='grid grid-cols-4 gap-6'>
            {auctions.results.map((auction: any) => (
              <AuctionCard auction={auction} key={auction.id} />
            ))}
          </div>
          <div className='flex justify-center'>
            <AppPagination
              currentPage={auctionParams.pageNumber}
              pageCount={auctions.pageCount}
              pageChanged={(page: number) => {
                dispatch(setPageNumber(page));
                window.scrollTo({ top: 0, behavior: 'smooth' });
              }}
            />
          </div>
        </>
      )}
    </>
  );
}
