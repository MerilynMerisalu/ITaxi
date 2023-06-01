interface IRideTimeFormData {

   
    scheduleId?: string,
    
    rideTimes?: string[],
    selectedRideTimes: string[] 
    isTaken: boolean

}

export type { IRideTimeFormData }