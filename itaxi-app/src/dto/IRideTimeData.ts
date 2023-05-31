interface IRideTimeData {
    scheduleId: string
    schedule: {
        
        startDateAndTime: string,
        endDateAndTime: string,
        shiftDurationTime: string
    },
    
    rideDateTime: string,
    isTaken: boolean
}

export type { IRideTimeData }