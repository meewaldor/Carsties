import { getCurrentUser } from '@/actions/authActions';
import Details from '@/features/auctions/components/Details';
import React from 'react';

export default async function page({ params }: { params: { id: string } }) {
  const user = await getCurrentUser();
  return <Details id={params.id} user={user} />;
}
