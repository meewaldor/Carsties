'use server'

import { auth } from '@/auth';

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
        mileage: Math.floor(Math.random()*1000) + 1
    }
    const res = await fetch('http://localhost:6001/auctions', {
        method: 'PUT',
        headers: {},
        body: JSON.stringify(data)
    });

    if(!res.ok) return {status: res.status, message: res.statusText}

    return res.json();
}