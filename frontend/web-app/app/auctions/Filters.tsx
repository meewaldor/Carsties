import { useParamsStore } from '@/hooks/useParamsStore';
import { Button, ButtonGroup } from 'flowbite-react';
import React from 'react';
import { AiOutlineClockCircle, AiOutlineSortAscending } from 'react-icons/ai';
import { BsFillStopCircleFill } from 'react-icons/bs';

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

export default function Filters() {
  const pageSize = useParamsStore((state) => state.pageSize);
  const orderBy = useParamsStore((state) => state.orderBy);
  const setParams = useParamsStore((state) => state.setParams);
  return (
    <div className='flex justify-between items-center mb-4'>
      <div>
        <span className='uppercase text-sm text-gray-500 mr-2'>Order by</span>
        <ButtonGroup>
          {orderButtons.map(({ label, icon: Icon, value }) => (
            <Button
              key={value}
              onClick={() => setParams({ orderBy: value })}
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
              onClick={() => setParams({ pageSize: value })}
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
