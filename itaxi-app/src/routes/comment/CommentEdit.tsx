import React, { useState, useContext, useEffect } from 'react'
import CommentForm from './CommentForm'
import { useNavigate, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { CommentService } from '../../services/CommentService';
import { ICommentFormData } from '../../dto/ICommentFormData';

type Props = {}
const service = new CommentService();

const CommentEdit = (props: Props) => {
    const navigate = useNavigate()
    const [data, setData] = useState<ICommentFormData>({
        driveId: '',
        commentText: '',
        driveTimeAndDriver: ''
    })
    const { id } = useParams();
    const { jwtLoginResponse } = useContext(JwtContext);
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            service.details(id)
                .then(
                    response => {
                        console.log('Comment edit details:', response)
                        if (response) {
                           const formData = {
                            ...response,
                           }
                            setData(formData)
                        } else {
                            setData({
                                driveId: '',
                                commentText: '',
                                driveTimeAndDriver: ""
                            })
                        }
                    }
                )
        }

    }, [id, jwtLoginResponse]);
    const editAction = async (values: ICommentFormData) =>{
        console.log('values test:', values)
        if (id == null){
            throw new Error('Cannot edit without id')
        }
        const status = await service.edit(id, values)
        console.log('status:', status)
        if (status === 204 || status === 200) {
            console.log('status ok')
            navigate("/comments");
        } else {
            console.warn('Vehicle create not OK', status)
        }
    }
    return (
        <div className="container">
            <main role="main" className="pb-3">
            <h1>Edit</h1>

                <h4>Comment</h4>
                <hr />
               <div className="row">
          <div className="col-md-4">
                        <CommentForm
                            action={editAction}
                            initialValues={data}
                            driveDisabled
                            buttonLabel='Save'
                        />
                    </div>
                </div>

                <div>
                    <a href="/CustomerArea/Comments">Back to List</a>
                </div>


            </main>
        </div>
    )
}

export default CommentEdit