import React from 'react'
import { Link, useNavigate } from 'react-router-dom';
import { ICommentFormData } from '../../dto/ICommentFormData';
import { CommentService } from '../../services/CommentService';
import CommentForm from './CommentForm';


const service = new CommentService();

const CommentCreate = () => {
  const navigate = useNavigate();
  const createAction = async (values: ICommentFormData) => {
    console.log("values test:", values);
    const status = await service.create(values);
    console.log("status:", status);
    if (status === 201 || status === 200) {
      console.log("status ok");
      navigate("/comments");
    } else {
      console.warn("Comment create not OK", status);
    }
  };
  return (
    <div className="container">
      <main role="main" className="pb-3">
        <h1>Create</h1>

        <h4>Comment</h4>
        <hr />
        <div className="row">
          <div className="col-md-4">
            <CommentForm
              action={createAction}
              initialValues={{
                driveId: "",
                commentText: ""
              }}
              buttonLabel='Create'
            />
          </div>
        </div>

        <div>
          <Link to={"/comments"}>Back to List</Link>
        </div>
      </main>
    </div>
  );
};




export default CommentCreate