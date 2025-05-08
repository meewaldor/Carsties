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

export async function getDetailedViewData(id: string) : Promise<Auction> {
  return await fetchWrapper.get(`auctions/${id}`);
}
export async function updateAuction(data: FieldValues, id: string) {
  return await fetchWrapper.put(`auctions/${id}`, data);
}
export async function deleteAuction(id: string) {
  return await fetchWrapper.del(`auctions/${id}`);
}