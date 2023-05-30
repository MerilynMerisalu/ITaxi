import React, { useContext, useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { IComment } from '../../domain/IComment';
import { CommentService } from '../../services/CommentService';


const CommentDetails = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IComment | null>(null)
    const { language } = useContext(JwtContext)
    const commentService = new CommentService();
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            commentService.details(id)
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

    }, [id, language]);

    function pad (s: number) {
        const padded = `0${s}`
        return padded.slice(-2)
    }
    console.log('language', language)

    function formatDate (iso: string) {
        const date = new Date(iso)
        const year = pad(date.getFullYear())
        const month = pad(date.getMonth() + 1)
        const day = pad(date.getDate())
        const hours = pad(date.getHours())
        const minutes = pad(date.getMinutes())

        if (language === 'en-GB') {
            return `${year}-${month}-${day} ${hours}:${minutes}`
        }
        if (language === 'et') {
            return `${day}.${month}.${year} ${hours}:${minutes}`
        }
    }

  return (
    <div className="container">
    <main role="main" className="pb-3">
        
<h1>Details</h1>

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
</div>
<div>
    <Link to={"/comments"}>Edit</Link> |
    <Link to={"/comments"}>Back to List</Link>
</div>
    </main>
</div>
  )
}

export default CommentDetails