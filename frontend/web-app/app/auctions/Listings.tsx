import React from 'react';
import AuctionCard from './AuctionCard';

async function getData() {
  const res = await fetch('http://localhost:6001/search');
  if (!res.ok) throw new Error('Fail to fetch data');
  return res.json();
}

export default async function Listings() {
  const data = await getData();
  return (
    <div>
      {data &&
        data.results.map((auction: any) => (
          <AuctionCard auction={auction} key={auction.id} />
        ))}
    </div>
  );
}
