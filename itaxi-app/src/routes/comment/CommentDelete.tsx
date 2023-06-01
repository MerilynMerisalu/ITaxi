import React, { FormEvent, useContext, useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { IComment } from '../../domain/IComment';
import { CommentService } from '../../services/CommentService';

const commentService = new CommentService();
const CommentDelete = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IComment | null>(null)
    const navigate = useNavigate()
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            commentService.deleteDetails(id)
                .then(
                    response => {
                        console.log(`Comment: ${response}`)
                        if (response)
                            setData(response)
                        else {
                            setData(null)
                        }
                    }
                )
        }

    }, [id, jwtLoginResponse, commentService]);
    
    const deleteAction = async (event: FormEvent) =>{
        event.preventDefault()
        console.log('deleteAction id test:', id)
        const status = await commentService.delete(id)
        console.log('deleteAction status:', status)
        if (status === 204 || status === 200) {
            console.log('status ok')
            navigate('/comments')
        } else {
            console.warn('Comment delete not OK', status)
        }
    }

  return (
    <div className="container">
    <main role="main" className="pb-3">
        
<h1>Delete</h1>

<h3>Are You Sure You Want To Delete This? </h3>
<div>
    <h4>Comment</h4>
    <hr/>
    
<dl className="row">
    <dt className="col-sm-2">
        Drive
    </dt>
    <dd className="col-sm-10">
        30/05/2023 03:31
    </dd>
    <dt className="col-sm-2">
        Driver
    </dt>
    <dd className="col-sm-10">
        Paju Toomas
    </dd>
    <dt className="col-sm-2">
        Comment
    </dt>
    <dd className="col-sm-10">
        J&#xE4;in teenusega rahule!
    </dd>
</dl>

    <form onSubmit={deleteAction}>
        <input type="hidden" id="Id" name="Id" value="e78742f3-5a89-4d62-da20-08db6059cdce" />
        <input type="submit" value="Delete" className="btn btn-danger"/> |
        <Link to={"/comments"}>Back to List</Link>
    <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0oMKO4znsfmJXXSCoxJceNG91cy19DTlNLiQ26cNdkEcYiBSq8TI0t0c0fgUGyTcuK4X156wqT2rN6aJw-EIPas5_wHQR7QrfLqxYBOIAJj3s9WEnezP4ziPgeU9Q--EFSdxjVXxW0Ogx1pnJHmPf_DR7PDzyJpBktRyXLJVttpGA" /></form>
</div>
    </main>
</div>
  )
}

export default CommentDelete