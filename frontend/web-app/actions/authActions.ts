'use server'

import { auth } from '@/auth';
import { fetchWrapper } from '@/lib/fetchWrapper';

export async function getCurrentUser() {
    try {
        const session = await auth();

        if(!session) return null;

        return session.user;
    } catch (error) {
        return null;
    }
}


export async function updateAuctionTest() {
    const data = {
        mileage: Math.floor(Math.random()*1000) + 1,
    };

    return await fetchWrapper.put('auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', data);
}