import * as React from 'react';
import 'dayjs/locale/et'
import 'dayjs/locale/en-gb'

import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import { IdentityService } from '../services/IdentityService';
import { useNavigate } from 'react-router-dom';
  let language = IdentityService.getLanguage();
console.log(language);
language = language.slice(0,2)
console.log(language) 
 
export default function BasicDateTimePicker() {
    const locales = [ 'en-gb', 'et'];
    type LocaleKey = (typeof locales)[number];
  const [locale, setLocale] = React.useState<LocaleKey>("en-gb");
 /*  if (language === "et") {
    setLocale("et")
    /* window.location.reload() */
  /* }
  else {
    setLocale("en-gb") */
   /*  window.location.reload() */
  //} 
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale={locale} >
      <DemoContainer components={['DateTimePicker']}>
        <DateTimePicker label="" /* value={locale} onChange={() => setLocale(language)} *//>
      </DemoContainer>
    </LocalizationProvider>
  );
}
