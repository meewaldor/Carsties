'use client';

import { useEffect } from 'react';
import { useSession } from 'next-auth/react';
import { useDispatch } from 'react-redux';
import { setToken } from '@/features/auth/slices/authSlice';

export default function AuthSessionSync() {
  const { data: session } = useSession();
  const dispatch = useDispatch();

  useEffect(() => {
    if (session?.accessToken) {
      dispatch(setToken(session.accessToken));
    }
  }, [session, dispatch]);

  return null;
}
