import { FC } from "react";
import classNames from "classnames";

import "./User.styles.scss";

export type UserProps = {
  username: String;
  className?: String;
};

const User: FC<UserProps> = ({ className: classes, username }) => {
  const className = classNames("user", classes);
  return (
    <div className={className}>
      <div className="user__username">{username}</div>
    </div>
  );
};

export default User;
