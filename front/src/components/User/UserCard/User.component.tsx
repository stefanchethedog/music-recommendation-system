import { FC } from "react";
import classNames from "classnames";
import DeleteIcon from "@mui/icons-material/Delete"

import "./User.styles.scss";
import axios from "axios";
import { DELETE_USER } from "../../../endpoints";

export type UserProps = {
  id: String;
  username: String;
  className?: String;
};

const User: FC<UserProps> = ({ className: classes, username, id }) => {
  const className = classNames("user", classes);
  const handleDeleteUser = async () => {
    axios.delete(DELETE_USER(id)).then().catch();
  }
  return (
    <div className={className}>
      <DeleteIcon className="user__delete" onClick={handleDeleteUser}/>
      <div className="user__username">{username}</div>
    </div>
  );
};

export default User;
