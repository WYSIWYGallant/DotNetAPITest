import React, { useEffect, useState } from "react";

function App() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [formData, setFormData] = useState({ id: "", name: "", email: "" });
    const [isEditing, setIsEditing] = useState(false);

    // Fetch data from the API
    useEffect(() => {
        fetch("https://jsonplaceholder.typicode.com/users")
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Failed to fetch data");
                }
                return response.json();
            })
            .then((data) => {
                setData(data);
                setLoading(false);
            })
            .catch((err) => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    // Handle input changes for the form
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    // Handle create or update operation
    const handleSubmit = (e) => {
        e.preventDefault();
        if (isEditing) {
            setData((prev) =>
                prev.map((item) =>
                    item.id === formData.id ? { ...item, ...formData } : item
                )
            );
        } else {
            setData((prev) => [...prev, { ...formData, id: Date.now() }]);
        }
        setFormData({ id: "", name: "", email: "" });
        setIsEditing(false);
    };

    // Handle delete operation
    const handleDelete = (id) => {
        setData((prev) => prev.filter((item) => item.id !== id));
    };

    // Handle edit operation
    const handleEdit = (item) => {
        setFormData(item);
        setIsEditing(true);
    };

    return (
        <div style={{ padding: "20px", fontFamily: "Arial, sans-serif" }}>
            <h1>CRUD Operations Example</h1>

            {/* Create/Edit Form */}
            <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>
                <input
                    type="text"
                    name="name"
                    value={formData.name}
                    onChange={handleInputChange}
                    placeholder="Name"
                    required
                />
                <input
                    type="email"
                    name="email"
                    value={formData.email}
                    onChange={handleInputChange}
                    placeholder="Email"
                    required
                />
                <button type="submit">{isEditing ? "Update" : "Create"}</button>
            </form>

            {/* Loading, Error, or Data Table */}
            {loading && <p>Loading data...</p>}
            {error && <p style={{ color: "red" }}>Error: {error}</p>}

            {!loading && !error && (
                <table
                    style={{
                        border: "1px solid black",
                        borderCollapse: "collapse",
                        width: "100%",
                    }}
                >
                    <thead>
                        <tr>
                            <th style={{ border: "1px solid black", padding: "8px" }}>ID</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Name</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Email</th>
                            <th style={{ border: "1px solid black", padding: "8px" }}>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map((item) => (
                            <tr key={item.id}>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.id}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.name}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>{item.email}</td>
                                <td style={{ border: "1px solid black", padding: "8px" }}>
                                    <button onClick={() => handleEdit(item)}>Edit</button>
                                    <button onClick={() => handleDelete(item.id)}>Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default App;
