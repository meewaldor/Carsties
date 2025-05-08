'use server'

import { fetchWrapper } from '@/lib/fetchWrapper';
import { Auction, PagedResult } from '@/types';

export async function getData(query: string): Promise<PagedResult<Auction>> {
  return await fetchWrapper.get(`search${query}`);
    
  }