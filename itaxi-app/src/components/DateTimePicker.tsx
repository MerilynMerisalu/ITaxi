import * as React from 'react';
import 'dayjs/locale/et'
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';

export default function BasicDateTimePicker() {
    const locales = ['en', 'en-gb', 'de', 'et'];
    type LocaleKey = (typeof locales)[number];
  const [locale, setLocale] = React.useState<LocaleKey>('et');

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale={locale} >
      <DemoContainer components={['DateTimePicker']}>
        <DateTimePicker label="Schedule Start Date and Time" />
      </DemoContainer>
    </LocalizationProvider>
  );
}
