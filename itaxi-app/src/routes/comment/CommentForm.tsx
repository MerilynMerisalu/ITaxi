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
import { DriveService } from "../../services/DriveService";
import { IDrive } from "../../domain/IDrive";

const service = new CommentService();
const driveServise = new DriveService();
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
    const [drives, setdrives] = useState<IDrive[]>();

    useEffect(() => {
        async function downloadDrives() {
            const drives = await driveServise.getAll();
            setdrives(drives);
        }

        async function download() {
            const promises = [
                downloadDrives(),
            ];
            await Promise.all(promises);
            console.log("download complete");
        }
        download();
        axios.defaults.headers.common["Accept-Language"] = language;
    }, [language]);
    function handleChange(
        event: ChangeEvent<HTMLSelectElement | HTMLInputElement | HTMLTextAreaElement>
    ) {
        setValues((currentValues) => {

            return {
                ...currentValues,
                 //[event.target.name]: value,
            };
        });
    }
    const driveViews = drives?.map((option) => (
        <option key={option.id} value={option.id}>
            {option.id}
        </option>
    ));


    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        await action(values);
    };

    return (
            <div className="row">
                    <div className="col-md-4">
                        <form onSubmit={handleSubmit}>
                            <div className="text-danger validation-summary-valid">
                                <ul>
                                    <li style={{display:"none"}}></li>
                            </ul>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="DriveId">Drive</label>
                                <select className="form-control" id="DriveId" name="driveId"
                                value={values.driveId}
                                onChange={(e) => handleChange(e)}>
                                    {drives ? <option>{driveViews}</option>: <option>There are no drives to select from</option>}
                                    
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="CommentText">Comment</label>
                                <textarea className="form-control" id="CommentText"  maxLength={1000} name="commentText"
                                value={values.commentText}
                                onChange={(e) => handleChange(e)}>
                                </textarea>
                                <span className="text-danger field-validation-valid"></span>
                            </div>

                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-primary" />
                            </div>
                            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0ppYpety6eJOy6DHmoTxKQ87Nxy1WnDU3VfSTfVslNzBUt_TitgZofCx5tiJMTbYoGIPsOYoRz_j59Fecwcnh0iuk9uIVVCyfs6omFAtfti65YjHWtl--Aq_GWY9DrgSW-QtA-NFq1EXU3BfXKPdjWX0SwtPtMcBMTZTcQEF5Ewtw" /></form>
                    </div>
                </div>

                
    );
};

export default CommentForm;
