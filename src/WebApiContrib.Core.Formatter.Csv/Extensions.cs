using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace WebApiContrib.Core.Formatter.Csv
{
    public static class Extensions
    {
        class IsNullVisitor : ExpressionVisitor
        {
            public bool IsNull { get; private set; }
            public object CurrentObject { get; set; }

            protected override Expression VisitMember(MemberExpression node)
            {
                base.VisitMember(node);
                if (CheckNull())
                    return node;
                var member = (PropertyInfo)node.Member;
                CurrentObject = member.GetValue(CurrentObject, null);
                CheckNull();
                return node;
            }

            private bool CheckNull()
            {
                if (CurrentObject == null)
                    IsNull = true;
                return IsNull;
            }
        }

        public static MemberExpression GetMemberExpression(this Expression expression)
        {
            switch (expression)
            {
                case MemberExpression e:
                    return e;
                case LambdaExpression e:
                    return GetMemberExpression(e.Body);
                case UnaryExpression e:
                    return GetMemberExpression(e.Operand);
                default:
                    return null;
            }
        }

        public static string GetPropertyPath(this MemberExpression memberExpression)
        {
            var path = new StringBuilder();
            do
            {
                if (path.Length > 0)
                    path.Insert(0, ".");
                path.Insert(0, memberExpression.Member.Name);
                memberExpression = GetMemberExpression(memberExpression.Expression);
            }
            while (memberExpression != null);
            return path.ToString();
        }

        public static bool WillThrowNullReferenceException(this Expression expression, object entity)
        {
            var visitor = new IsNullVisitor
            {
                CurrentObject = entity
            };
            var memberExpression = expression.GetMemberExpression();
            visitor.Visit(memberExpression);
            return visitor.IsNull;
        }
    }
}