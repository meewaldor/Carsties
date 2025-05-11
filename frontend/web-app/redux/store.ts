import { auctionApi } from '@/features/auctions/api/AuctionApi';
import { auctionSlice } from '@/features/auctions/slices/auctionSlice';
import { authSlice } from '@/features/auth/slices/authSlice';
import { bidApi } from '@/features/bids/api/BidApi';
import { configureStore } from '@reduxjs/toolkit';
import { setupListeners } from '@reduxjs/toolkit/query';
import { persistStore } from 'redux-persist';

export const store = configureStore({
  reducer: {
    [auctionApi.reducerPath]: auctionApi.reducer,
    [bidApi.reducerPath]: bidApi.reducer,
    auction: auctionSlice.reducer,
    auth: authSlice.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({ serializableCheck: false }).concat(
      auctionApi.middleware,
      bidApi.middleware
    ),
});

export const persistor = persistStore(store);

setupListeners(store.dispatch);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
