import { IBaseEntity } from "./Base/IBaseEntity";

export interface IDrive extends IBaseEntity {
    commentId: string,
    comment: {
        commentText: string
    }

}