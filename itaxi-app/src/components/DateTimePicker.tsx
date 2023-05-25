import * as React from 'react';
import 'dayjs/locale/et'
import 'dayjs/locale/en-gb'


import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
 import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import { JwtContext } from '../routes/Root';
  
 
export default function BasicDateTimePicker( {onChange, type }: any) {
  const { language } = React.useContext(JwtContext);
   const handlerChange = (e:any) => {
    console.log(e)
    onChange(e.$d, type)
  } 

  const localeHelper = () => {
    return language?.toLowerCase() 
  }

  console.log('*** LANG', language)
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale={localeHelper()} >
      
       <DemoContainer components={['DateTimePicker']}> 
        
        <DateTimePicker  onChange={handlerChange} disablePast
        />
        
     </DemoContainer> 
    </LocalizationProvider>
  );
}
