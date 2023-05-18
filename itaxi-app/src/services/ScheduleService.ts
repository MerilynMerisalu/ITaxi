import { ISchedule } from "../domain/ISchedule";

import { BaseEntityService } from "./BaseEntityService";


export class ScheduleService extends BaseEntityService<ISchedule> {
    constructor() {
        super('v1/driverarea/schedules')
    }
}