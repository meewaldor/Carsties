'use client';
import { updateAuctionTest } from '@/actions/authActions';
import { Button } from 'flowbite-react';
import React, { useState } from 'react';

export default function AuthTest() {
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<any>();

  function doUpdate() {
    setResult(undefined);
    setLoading(true);
    updateAuctionTest()
      .then((res) => setResult(res))
      .catch((err) => setResult(err))
      .finally(() => setLoading(false));
  }
  return (
    <div className='flex items-center gap-4'>
      <Button outline disabled={loading} onClick={doUpdate}>
        Test Auth
      </Button>
      <div>{JSON.stringify(result, null, 2)}</div>
    </div>
  );
}
