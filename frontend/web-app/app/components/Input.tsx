import { Label, TextInput } from 'flowbite-react';
import React from 'react';
import { useController, UseControllerProps } from 'react-hook-form';

type Props = {
  label: string;
  type?: string;
  showLabel?: boolean;
} & UseControllerProps;

export default function Input(props: Props) {
  const { field, fieldState } = useController({ ...props, defaultValue: '' });
  return (
    <div className='mb-3'>
      {props.showLabel && (
        <div className='mb-2 block'>
          <Label htmlFor={field.name}>{props.label}</Label>
        </div>
      )}
      <TextInput
        {...props}
        {...field}
        type={props.type || 'text'}
        placeholder={props.label}
        color={
          fieldState.error ? 'failure' : !fieldState.isDirty ? '' : 'success'
        }
      />
      {fieldState.error && (
        <p className='mt-1 text-sm text-red-500'>{fieldState.error.message}</p>
      )}
    </div>
  );
}
