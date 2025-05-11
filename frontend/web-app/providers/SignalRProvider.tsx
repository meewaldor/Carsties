'use client';
import React, { ReactNode, useCallback, useEffect, useRef } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { useParams } from 'next/navigation';
import { Bid } from '@/features/bids/types';
import { useAppDispatch, useAppSelector } from '@/redux/hooks';
import { auctionApi } from '@/features/auctions/api/AuctionApi';
import { Auction } from '@/types';
import { User } from 'next-auth';
import toast from 'react-hot-toast';
import AuctionCreatedToast from '@/components/AuctionCreatedToast';
import { AuctionFinished } from '@/features/auctions/types';
import { getDetailedViewData } from '@/actions/auctionActions';
import AuctionFinishedToast from '@/components/AuctionFinishedToast';
type Props = {
  children: ReactNode;
  user: User | null;
};
export default function SignalRProvider({ children, user }: Props) {
  const connection = useRef<HubConnection | null>(null);
  const params = useParams<{ id: string }>();
  const dispatch = useAppDispatch();
  const auctionParams = useAppSelector((state) => state.auction);

  const handleBidPlaced = useCallback(
    (bid: Bid) => {
      if (bid.bidStatus.includes('Accepted')) {
        dispatch(
          auctionApi.util.updateQueryData(
            'getAuctions',
            auctionParams,
            (draft: any) => {
              const targetAuction: Auction = draft.results.find(
                (a: any) => a.id === bid.auctionId
              );
              if (targetAuction) {
                targetAuction.currentHighBid = bid.amount;
              }
            }
          )
        );
      }
    },
    [auctionParams, dispatch]
  );

  const handleAuctionCreated = useCallback(
    (auction: Auction) => {
      if (user?.username !== auction.seller) {
        return toast(<AuctionCreatedToast auction={auction} />, {
          duration: 10000,
        });
      }
    },
    [user?.username]
  );

  const handleAuctionFinished = useCallback(
    (finishedAuction: AuctionFinished) => {
      const auction = getDetailedViewData(finishedAuction.auctionId);
      return toast.promise(
        auction,
        {
          loading: 'Loading',
          success: (auction) => (
            <AuctionFinishedToast
              auction={auction}
              finishedAuction={finishedAuction}
            />
          ),
          error: (err) => 'Auction finished',
        },
        { success: { duration: 10000, icon: null } }
      );
    },
    []
  );

  useEffect(() => {
    if (!connection.current) {
      connection.current = new HubConnectionBuilder()
        .withUrl('http://localhost:6001/notifications')
        .withAutomaticReconnect()
        .build();

      connection.current
        .start()
        .then(() => 'Connected to notification hub')
        .catch((err) => console.log(err));

      connection.current.on('BidPlaced', handleBidPlaced);
      connection.current.on('AuctionCreated', handleAuctionCreated);
      connection.current.on('AuctionFinished', handleAuctionFinished);

      return () => {
        connection.current?.off('BidPlaced', handleBidPlaced);
        connection.current?.off('AuctionCreated', handleAuctionCreated);
        connection.current?.off('AuctionFinished', handleAuctionFinished);
      };
    }
  }, [handleBidPlaced, handleAuctionCreated, handleAuctionFinished]);
  return children;
}
