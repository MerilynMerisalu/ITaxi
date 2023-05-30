import {
    ChangeEvent,
    useState,
    useEffect,
    useContext,
    FormEvent,
} from "react";
import { CommentService } from "../../services/CommentService";
import { ICommentFormData } from "../../dto/ICommentFormData";
import { JwtContext } from "../Root";
import axios from "axios";
import { IBooking } from "../../domain/IBooking";
import { BookingService } from "../../services/BookingService";
import { Link } from "react-router-dom";

const service = new CommentService();
const bookingServise = new BookingService();
const CommentForm = ({
    action,
    initialValues,
}: {
    action: (values: ICommentFormData) => Promise<void>;
    initialValues: ICommentFormData
}) => {
    const [values, setValues] = useState(initialValues);
    useEffect(() => {
        console.log('values effect', initialValues)
        setValues(initialValues)
    }, [initialValues])

    const { language } = useContext(JwtContext);
    const [bookings, setBookings] = useState<IBooking[]>();

    useEffect(() => {
        async function downloadBookings() {
            const bookings = await bookingServise.getAll();
            setBookings(bookings);
        }

        async function download() {
            const promises = [
                downloadBookings(),
            ];
            await Promise.all(promises);
            console.log("download complete");
        }
        download();
        axios.defaults.headers.common["Accept-Language"] = language;
    }, [language]);
    function handleChange(
        event: ChangeEvent<HTMLSelectElement | HTMLInputElement>
    ) {
        setValues((currentValues) => {

            return {
                ...currentValues,
                // [event.target.name]: value,
            };
        });
    }
    const bookingViews = bookings?.map((option) => (
        <option key={option.id} value={option.id}>
            {option.id}
        </option>
    ));


    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        await action(values);
    };

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Create</h1>

                <h4>Comment</h4>
                <hr />
                <div className="row">
                    <div className="col-md-4">
                        <form action="/CustomerArea/Comments/Create" method="post">
                            <div className="text-danger validation-summary-valid" ><ul><li style={{ display: "none" }}></li>
                            </ul></div>
                            <div className="form-group">
                                <label className="control-label" html-for="DriveId">Drive</label>
                                <select className="form-control" id="DriveId" name="DriveId">
                                    <option>There are no drives to select from</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="CommentText">Comment</label>
                                <textarea className="form-control" id="CommentText" maxLength={1000} name="CommentText">
                                </textarea>
                                <span className="text-danger field-validation-valid"></span>
                            </div>

                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-primary" />
                            </div>
                            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0p1MiE2OM2XrLl0JPFO7wuu3rHeq4N9JbEO6-sS9wqYZPyxEzJ1DbjTanF_oLbhCi-ldwEfWRz4DjZ64Y3x2482IEpve9BsP0R5fR50QVGEL759d5e4kz70oeLS46PIQgApimZYb2GyBg_Jvr-AG_YPArAiX92n8kZ68muHbJpBSA" /></form>
                    </div>
                </div>

                <div>
                    <Link to={"/comments"}>Back to List</Link>
                </div>
            </main>
        </div>
    );
};

export default CommentForm;
