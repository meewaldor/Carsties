import { AuctionParams } from '@/types/params';
import { createSlice } from '@reduxjs/toolkit';

const initialState: AuctionParams = {
  pageNumber: 1,
  pageSize: 8,
  filterBy: 'live',
  orderBy: 'make',
  searchTerm: '',
  seller: undefined,
  winner: undefined,
};

export const auctionSlice = createSlice({
  name: 'auctionSlice',
  initialState,
  reducers: {
    setPageNumber(state, action) {
      state.pageNumber = action.payload;
    },
    setPageSize(state, action) {
      state.pageSize = action.payload;
    },
    setOrderBy(state, action) {
      state.orderBy = action.payload;
      state.pageNumber = 1;
    },
    setFilterBy(state, action) {
      state.filterBy = action.payload;
      state.pageNumber = 1;
    },
    setSearchTerm(state, action) {
      state.searchTerm = action.payload;
      state.pageNumber = 1;
    },
    setSeller(state, action) {
      state.seller = action.payload;
      state.pageNumber = 1;
    },
    setWinner(state, action) {
      state.winner = action.payload;
      state.pageNumber = 1;
    },
    resetParams() {
      return initialState;
    },
  },
});

export const {
  setOrderBy,
  setPageNumber,
  setPageSize,
  setSearchTerm,
  setSeller,
  setWinner,
  setFilterBy,
  resetParams,
} = auctionSlice.actions;
