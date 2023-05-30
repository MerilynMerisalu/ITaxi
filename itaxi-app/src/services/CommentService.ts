import { redirect } from "react-router-dom";
import { IComment } from "../domain/IComment";
import { BaseEntityService } from "./BaseEntityService";
import { IdentityService } from "./IdentityService";

 export class CommentService extends BaseEntityService<IComment> {
    constructor(){
        super('v1/customerArea/comments');
    }  
    
}