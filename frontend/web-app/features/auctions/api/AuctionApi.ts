// Need to use the React-specific entry point to import createApi
import { baseQueryWithErrorHandling } from '@/lib/baseApi';
import { filterEmptyValues } from '@/lib/utils';
import { Auction, PagedResult } from '@/types';
import { AuctionParams } from '@/types/params';
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { FieldValue } from 'react-hook-form';
import { CreateAuction } from '../types';

// Define a service using a base URL and expected endpoints
export const auctionApi = createApi({
  reducerPath: 'auctionApi',
  baseQuery: baseQueryWithErrorHandling,
  tagTypes: ['Auctions'],
  endpoints: (builder) => ({
    getAuctions: builder.query<PagedResult<Auction>, AuctionParams>({
      query: (auctionParams) => ({
        url: `/search`,
        params: filterEmptyValues(auctionParams),
      }),
      providesTags: ['Auctions'],
    }),
    createAuction: builder.mutation<Auction, CreateAuction>({
      query: (auction) => ({
        url: '/auctions',
        method: 'POST',
        body: auction,
      }),
      onQueryStarted: async (_, { dispatch, queryFulfilled }) => {
        await queryFulfilled;
        dispatch(auctionApi.util.invalidateTags(['Auctions']));
      },
    }),
  }),
});

// Export hooks for usage in functional components, which are
// auto-generated based on the defined endpoints
export const { useGetAuctionsQuery, useCreateAuctionMutation } = auctionApi;
