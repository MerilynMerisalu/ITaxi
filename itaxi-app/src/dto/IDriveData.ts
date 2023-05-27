interface IDriveData {
    booking: {
        schedule: {
            startDateAndTime: string,
            endDateAndTime: string,
            shiftDurationTime: string
        },
        customer: {
            appUser: {
                firstName: string,
                lastName: string,
                lastAndFirstName: string
            },
            disabilityType: {
                disabilityTypeName: string
            }
        },
        pickUpDateAndTime: string,
        city: {
            cityName: string
        },
        pickupAddress: string,
        destinationAddress: string,
        vehicle: {
            vehicleType: {
                vehicleTypeName: string
            },
            vehicleIdentifier: string
        },
        numberOfPassengers: string,
        hasAnAssistant: boolean,
        statusOfBooking: number
    },
    statusOfDrive: number,
    comment: {
        commentText: string
    }
}

export type { IDriveData }