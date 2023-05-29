interface IRideTimeData {
    schedule: {
        
        startDateAndTime: string,
        endDateAndTime: string,
        shiftDurationTime: string
    },
    
    rideDateTime: string,
    isTaken: boolean
}

export type { IRideTimeData }