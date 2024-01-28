import { FC } from "react";
import { UserForm } from "../../components";

import "./User.styles.scss";

const UserPage: FC = () => {
  return (
    <div className="user-page">
      <UserForm />
    </div>
  );
};

export default UserPage;
