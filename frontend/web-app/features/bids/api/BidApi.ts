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
  }),
});

export const { useGetBidsForAuctionQuery } = bidApi;
