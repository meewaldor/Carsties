'use client';
import React from 'react';
import { FieldValues, useForm } from 'react-hook-form';
import { usePlaceBidForAuctionMutation } from '../api/BidApi';
import toast from 'react-hot-toast';
import { numberWithCommas } from '@/lib/utils';

type Props = {
  auctionId: string;
  highBid: number;
};
export default function BidForm({ auctionId, highBid }: Props) {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm();

  const [placeBidForAuction] = usePlaceBidForAuctionMutation();
  async function onSubmit(data: FieldValues) {
    try {
      await placeBidForAuction({ auctionId: auctionId, amount: +data.amount });
      reset();
    } catch (error: any) {
      console.log(error);
      toast.error(error.status + ' ' + error.message);
    }
  }
  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className='flex items-center border-2 rounded-lg py-2'
    >
      <input
        type='number'
        {...register('amount')}
        className='input-custom text-sm text-gray-600'
        placeholder={`Enter your bid (minimum bid is $${numberWithCommas(
          highBid + 1
        )})`}
      />
    </form>
  );
}
