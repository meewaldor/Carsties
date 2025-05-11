// Need to use the React-specific entry point to import createApi
import { baseQueryWithErrorHandling } from '@/lib/baseApi';
import { filterEmptyValues } from '@/lib/utils';
import { Auction, PagedResult } from '@/types';
import { AuctionParams } from '@/types/params';
import { createApi } from '@reduxjs/toolkit/query/react';
import { FieldValues } from 'react-hook-form';

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
    getAuctionDetails: builder.query<Auction, string>({
      query: (id) => `auctions/${id}`,
    }),
    createAuction: builder.mutation<Auction, FieldValues>({
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
    updateAuction: builder.mutation<void, { id: string; data: FieldValues }>({
      query: ({ id, data }) => ({
        url: `auctions/${id}`,
        method: 'PUT',
        body: data,
      }),
      onQueryStarted: async (_, { dispatch, queryFulfilled }) => {
        await queryFulfilled;
        dispatch(auctionApi.util.invalidateTags(['Auctions']));
      },
    }),
    deleteAuction: builder.mutation<void, string>({
      query: (id) => ({
        url: `/auctions/${id}`,
        method: 'DELETE',
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
export const {
  useGetAuctionsQuery,
  useCreateAuctionMutation,
  useGetAuctionDetailsQuery,
  useUpdateAuctionMutation,
  useDeleteAuctionMutation,
} = auctionApi;
