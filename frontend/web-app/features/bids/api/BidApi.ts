import { baseQueryWithErrorHandling } from '@/lib/baseApi';
import { createApi } from '@reduxjs/toolkit/query/react';
import { Bid } from '../types';

export const bidApi = createApi({
  reducerPath: 'bidApi',
  baseQuery: baseQueryWithErrorHandling,
  tagTypes: ['Bids'],
  endpoints: (builder) => ({
    getBidsForAuction: builder.query<Bid[], string>({
      query: (id) => ({ url: `/bids/${id}` }),
      providesTags: ['Bids'],
    }),
    placeBidForAuction: builder.mutation<
      void,
      { auctionId: string; amount: number }
    >({
      query: ({ auctionId, amount }) => ({
        url: `/bids?auctionId=${auctionId}&amount=${amount}`,
        method: 'POST',
      }),
      onQueryStarted: async (_, { dispatch, queryFulfilled }) => {
        await queryFulfilled;
        dispatch(bidApi.util.invalidateTags(['Bids']));
      },
    }),
  }),
});

export const { useGetBidsForAuctionQuery, usePlaceBidForAuctionMutation } =
  bidApi;
