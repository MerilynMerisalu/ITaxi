import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root';
import { IComment } from '../../domain/IComment';
import axios from 'axios';
import { CommentService } from '../../services/CommentService';
import { Link } from 'react-router-dom';

const commentService = new CommentService();
const CommentIndex = () => {
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState([] as IComment[])
    const { language } = useContext(JwtContext)

    useEffect(() => {
        axios.defaults.headers.common['Accept-Language'] = language;
        commentService.getAll()
            .then(
                response => {
                    console.log(response)
                    if (response)
                        setData(response)
                    else {
                        setData([])
                    }
                }
            )

    }, [language]);

    function pad(c: number) {
        const padded = `0${c}`
        return padded.slice(-2)
    }
    console.log('language', language)

    function formatDate(iso: string) {
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

    console.log('data', data)

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Index</h1>

                <p>
                    <Link to="create">Create New</Link>
                </p>
                <table className="table">
                    <thead>
                        <tr>
                            <th>
                                Drive Time
                            </th>

                            {/* <th>
                                Driver Name
                            </th>
                            <th>
                                Comment
                            </th> */}

                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map(c => (
                            <tr key={c.id}>
                                <td>
                                    {c.drive.booking.pickUpDateAndTime}
                                </td>

                                {/* <td>
                                    Paju Toomas
                                </td>

                                <td>
                                    J&#xE4;in teenusega rahule!
                                </td> */}

                                <td>
                                    <Link to={`/comment/edit/${c.id}`}>Edit</Link> |
                                    <Link to={`/comment/details/${c.id}`}>Details</Link> |
                                    <Link to={`/comment/delete/${c.id}`}>Delete</Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </main>
        </div>
    )
}

export default CommentIndex