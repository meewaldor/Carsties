import { useParamsStore } from '@/hooks/useParamsStore';
import { useAppDispatch, useAppSelector } from '@/redux/hooks';
import { Button, ButtonGroup } from 'flowbite-react';
import React from 'react';
import { AiOutlineClockCircle, AiOutlineSortAscending } from 'react-icons/ai';
import { BsFillStopCircleFill, BsStopwatchFill } from 'react-icons/bs';
import { GiFinishLine, GiFlame } from 'react-icons/gi';
import { setFilterBy, setOrderBy, setPageSize } from '../slices/auctionSlice';

const pageSizeButtons = [4, 8, 12];
const orderButtons = [
  {
    label: 'Alphabetical',
    icon: AiOutlineSortAscending,
    value: 'make',
  },
  {
    label: 'End date',
    icon: AiOutlineClockCircle,
    value: 'endingSoon',
  },
  {
    label: 'Recently Added',
    icon: BsFillStopCircleFill,
    value: 'new',
  },
];

const filterButtons = [
  {
    label: 'Live Auction',
    icon: GiFlame,
    value: 'live',
  },
  {
    label: 'Ending < 6 hours',
    icon: GiFinishLine,
    value: 'endingSoon',
  },
  {
    label: 'Completed',
    icon: BsStopwatchFill,
    value: 'finish',
  },
];

export default function Filters() {
  const { pageSize, filterBy, orderBy } = useAppSelector(
    (state) => state.auction
  );
  const dispatch = useAppDispatch();

  return (
    <div className='flex justify-between items-center mb-4'>
      <div>
        <span className='uppercase text-sm text-gray-500 mr-2'>Filter by</span>
        <ButtonGroup>
          {filterButtons.map(({ label, icon: Icon, value }) => (
            <Button
              key={value}
              onClick={() => dispatch(setFilterBy(value))}
              color={`${filterBy === value ? 'red' : 'gray'}`}
            >
              <Icon className='mr-3 h-4 w-4' />
              {label}
            </Button>
          ))}
        </ButtonGroup>
      </div>
      <div>
        <span className='uppercase text-sm text-gray-500 mr-2'>Order by</span>
        <ButtonGroup>
          {orderButtons.map(({ label, icon: Icon, value }) => (
            <Button
              key={value}
              onClick={() => dispatch(setOrderBy(value))}
              color={`${orderBy === value ? 'red' : 'gray'}`}
            >
              <Icon className='mr-3 h-4 w-4' />
              {label}
            </Button>
          ))}
        </ButtonGroup>
      </div>
      <div>
        <span className='uppercase text-sm text-gray-500 mr-2'>Page size</span>
        <ButtonGroup>
          {pageSizeButtons.map((value, i) => (
            <Button
              key={i}
              onClick={() => dispatch(setPageSize(value))}
              color={`${pageSize === value ? 'red' : 'gray'}`}
              className='focus:ring-0'
            >
              {value}
            </Button>
          ))}
        </ButtonGroup>
      </div>
    </div>
  );
}
