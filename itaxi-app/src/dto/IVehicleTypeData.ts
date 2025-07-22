interface IVehicleTypeData {
    vehicleTypeName: string
}

export type { IVehicleTypeData }

async getAll(): Promise<TEntity[] | undefined> {

    try {
      
      

        console.log('response', response);
        if (response.status === 200) {
          return response.data;

        } else {
          throw Error("User is not logged in");
        }

      

    } catch (e) {
      console.log('error: ', (e as Error).message);
      return undefined;
    }

  }

