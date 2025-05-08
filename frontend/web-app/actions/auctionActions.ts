'use server'

import { fetchWrapper } from '@/lib/fetchWrapper';
import { Auction, PagedResult } from '@/types';
import { FieldValues } from 'react-hook-form';

export async function getData(query: string): Promise<PagedResult<Auction>> {
  return await fetchWrapper.get(`search${query}`);
    
  }

export async function createAuction(data: FieldValues) {
  return await fetchWrapper.post('auctions', data);
}