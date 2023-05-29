import { AxiosError } from 'axios';
import { Document, Page } from 'react-pdf'
import React from 'react'

type Props = {}

const Print = (props: Props) => {
  const url = 'https://localhost:7026/DriverArea/Drives/Print';
  console.log('url', url)
  return (
    <div>
      Print
      {/* <Document file={url}>
        <Page pageNumber={1} />
      </Document> */}
    </div>
  )
}

export default Print