import * as React from 'react';
import 'dayjs/locale/et'
import 'dayjs/locale/en-gb'

import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import { JwtContext } from '../routes/Root';
  
 
export default function BasicDateTimePicker() {
  const { language } = React.useContext(JwtContext);
  
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale={language}>
      
      <DemoContainer components={['DateTimePicker']}>
        
        <DateTimePicker label="" />
        
      </DemoContainer>
    </LocalizationProvider>
  );
}
